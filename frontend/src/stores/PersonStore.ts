import { type Person } from "@/models/person";
import { ref } from "vue";
import { defineStore } from "pinia";
import useApi, { useApiRawRequest } from "@/models/api";

export const usePersonsStore = defineStore('PersonsStore', () => {
  const apiGetPersons = useApi<Person[]>('Person');
  const persons = ref<Person[]>([]);
  let allPersons: Person[] = [];

  const loadPersons = async () => {
    await apiGetPersons.request();
    
    if (apiGetPersons.response.value) {
      return apiGetPersons.response.value;
    }
    return [];
  };

  const load = async () => {
    allPersons = await loadPersons();
    persons.value = allPersons;
  };

  const getPersonById = (id: number) => {
    return allPersons.find((person) => person.id === id);
  };

  const addPerson = async (person: Person) => {
    const apiAddPerson = useApi<Person>('Person', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(person),
    });

    await apiAddPerson.request();
    if (apiAddPerson.response.value) {
      await load();
    }
  };

  const updatePerson = async (person: Person) => {
    const apiUpdatePerson = useApi<Person>(`Person/${person.id}`, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(person),
    });

    await apiUpdatePerson.request();
    if (apiUpdatePerson.response.value || apiUpdatePerson.status.value === 204) {
      await load();
    }
  };

  const deletePerson = async (person: Person) => {
    const deletePersonRequest = useApiRawRequest(`Person/${person.id}`, {
      method: 'DELETE',
    });

    const res = await deletePersonRequest();

    if (res.status === 204) {
      const index = persons.value.findIndex(p => p.id === person.id);
      if (index !== -1) {
        persons.value.splice(index, 1);
      }
    }
  };

  return { persons, load, getPersonById, addPerson, updatePerson, deletePerson };
});
