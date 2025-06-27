import { type People } from "@/models/people";
import { ref } from "vue";
import { defineStore } from "pinia";
import useApi, { useApiRawRequest } from "@/models/api";

export const usePeopleStore = defineStore('peopleStore', () => {
  const people = ref<People[]>([]);
  let allPeople: People[] = [];

  const loadEvents = async () => {
    const apiGetEvents = useApi<People[]>('people',{headers:{
      Accept: 'application/json',
      'Content-Type': 'application/json',
      Authorization:"Bearer "+localStorage.getItem('jwt')
    }});
    await apiGetEvents.request();

    if (apiGetEvents.response.value) {
      return apiGetEvents.response.value;
    }
    return [];
  };

  const load = async () => {
    allPeople = await loadEvents();
    allPeople = allPeople.sort((a, b) => a.id - b.id)
    people.value = allPeople;
  };
  const getEventById = (id: Number) => {
    return allPeople.find((people) => people.id === id);
  };


  const addPerson = async (person: People) => {
    const apiAddEvent = useApi<People>('people', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        Authorization:"Bearer "+localStorage.getItem('jwt')
      },
      body: JSON.stringify(person),
    }); 
    
    await apiAddEvent.request();
    if (apiAddEvent.response.value) {
      load();      
    }
  };
  const updatePerson = async (person: People) => {
    const apiUpdateEvent = useApi<People>('people/' + person.id, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        Authorization:"Bearer "+localStorage.getItem('jwt')
      },
      body: JSON.stringify(person),
    });

    await apiUpdateEvent.request();
    if (apiUpdateEvent.response.value) {
      load();      
    }
  };


  const deletePerson = async (person: People) => {
    const deleteEventRequest = useApiRawRequest(`people/${person.id}`, {
      method: 'DELETE',
      headers:{
        Authorization:"Bearer "+localStorage.getItem('jwt')
      }
    });
    const res = await deleteEventRequest();
    if (res.status === 204) {
      for (let index = 0; index < people.value.length; index++) {
        const element = people.value[index];
        if (element.id == person.id){
          people.value.splice(element.id, 1);
        }
      }
      let id = people.value.indexOf(person);
      if (id !== -1) {
        people.value.splice(id, 1);
      }

      id = people.value.indexOf(person);

      if (id !== -1) {
        people.value.splice(id, 1);
      }
      load();
    }
  };

  return { people, load, getEventById, addPerson, updatePerson, deletePerson };
});