import { type Note } from "@/models/note";
import { ref } from "vue";
import { defineStore } from "pinia";
import useApi, { useApiRawRequest } from "@/models/api";

export const useNoteStore = defineStore('noteStore', () => {
  const note = ref<Note[]>([]);
  let allNote: Note[] = [];

  const loadNotes = async (username:string) => {
    const apiGetEvents = useApi<Note[]>('privatenotes/getall?username='+username,{headers:{
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

  const load = async (username:string) => {
    allNote = await loadNotes(username);
    allNote = allNote.sort((a, b) => a.id - b.id)
    note.value = allNote;
  };
  const getEventById = (id: Number) => {
    return allNote.find((note) => note.id === id);
  };


  const addNote = async (person: Note,username:string) => {
    const apiAddEvent = useApi<Note>('privatenotes/add?username='+username, {
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
      load(username);      
    }
  };
  const updateNote = async (person: Note,username:string) => {
    const apiUpdateEvent = useApi<Note>('privatenotes/update?username=' + username, {
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
      load(username);      
    }
  };


  const deleteNote = async (person: Note,username:string) => {
    const deleteEventRequest = useApiRawRequest(`privatenotes/delete/?username=${username}&id=${person.id}`, {
      method: 'DELETE',
      headers:{
        Accept: 'application/json',
        'Content-Type': 'application/json',
        Authorization:"Bearer "+localStorage.getItem('jwt')
      },
    });
    const res = await deleteEventRequest();
    if (res.status === 204) {
      for (let index = 0; index < note.value.length; index++) {
        const element = note.value[index];
        if (element.id == person.id){
          note.value.splice(element.id, 1);
        }
      }
      let id = note.value.indexOf(person);
      if (id !== -1) {
        note.value.splice(id, 1);
      }

      id = note.value.indexOf(person);

      if (id !== -1) {
        note.value.splice(id, 1);
      }
      load(username);
    }
  };

  return { note, load, getEventById, addNote, updateNote, deleteNote };
});