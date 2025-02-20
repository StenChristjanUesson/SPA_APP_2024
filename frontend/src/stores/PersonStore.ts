import { type Person } from "@/models/Person";
import { ref } from "vue";
import { defineStore } from "pinia";
import useApi, { useApiRawRequest } from "@/models/api";

export const usePersonsStore = defineStore('personsStore', () => {
  const apiGetPersons = useApi<Person[]>('persons');
  const persons = ref<Person[]>([]);
  let allPersons: Person[] = [];

  const loadPersons = async () => {
    await apiGetPersons.request();
    
    if (apiGetPersons.response.value) {
      return apiGetPersons.response.value;
    }
    return [];
  };

  const load = async() => {
    allPersons = await loadPersons();
    persons.value = allPersons;
  };

  const getPersonById = (id: Number) => {
    return allPersons.find((person) => person.id === id);
  };

  const addPerson = async (person: Person) => {
    const apiAddPerson = useApi<Person>('persons', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(person),
    }); 
    
    await apiAddPerson.request();
    if (apiAddPerson.response.value) {
        load();
    }
  };

  const updatePerson = async (person: Person) => {
    const apiUpdatePerson = useApi<Person>('persons/' + person.id, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(person),
    });

    await apiUpdatePerson.request();
    if (apiUpdatePerson.response.value) {
      load();
    }    
  };

  const deletePerson = async (person: Person) => {
    const deletePersonRequest = useApiRawRequest(`persons/${person.id}`, {
      method: 'DELETE',
    });

    const res = await deletePersonRequest();

    if (res.status === 204) {
      const id = persons.value.indexOf(person);
      
      if (id !== -1) {
        persons.value.splice(id, 1);
      }
    }
  };

  return { persons, load, getPersonById, addPerson, updatePerson, deletePerson };
});
