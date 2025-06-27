<template>
  <div class="about">
    <h1 v-text="usrName"></h1>
    <Message v-if="failedLogin" severity="error" v-text="errMsg"></Message>
    <div class="noteList">
      <DataTable :value="note" v-if="note.length > 0 ">
        <!-- <Column field="id" header="Id" style="color: black; "/> -->
        <Column field="name" header="Name" style="color: black; "/>
        <Column field="description" header="Description" style="color: black; "/>
        <Column header="Actions" style="color: black;">
          <template #body="{ data }">
              <Button type="submit" variant="text" severity="info" label="Uuenda" @click="showUpdate(data.id)"/>
              <Button type="submit" variant="text" severity="danger" label="Kustuta" @click="deleteNote({id:data.id,name:data.name,description:data.description})"/>
            <!-- <Button type="submit" variant="text" severity="info" label="Registeeri siia" @click="RegToEvent(data.id)"/> -->
        </template>
        </Column>
      </DataTable>
      <div v-else>No notes!</div>
    </div>
  </div>
  <div>
  <br><br><br>
      <div v-if="!update">
        <InputText name="name" type="text" placeholder="Name" v-model="addName"/><br><br>
        <InputText name="email" type="text" placeholder="Description" v-model="addEmail"/><br><br>
        <Button type="submit" severity="secondary" label="Lisa uus note" @click="addNew"/>
      </div>
      <div v-if="update">
        <InputText name="name" type="text" placeholder="Name" v-model="updateName"/><br><br>
        <InputText name="email" type="text" placeholder="Description" v-model="updateEmail"/><br><br>
        <Button type="submit" severity="secondary" label="Uuenda infot" @click="updatePerson"/><br><br><br>
        <Button type="submit" severity="danger" label="Tühista uuendamine" @click="cancelUpdate"/>

    </div>
  </div>
</template>

<script setup lang="ts">
import { useRoute,useRouter} from "vue-router";
const router = useRouter()
if(!localStorage.getItem("jwt")){
    router.push("/")
}
import { computed } from 'vue'
import { type Note } from '@/models/note';
import { useNoteStore } from "@/stores/eventsStore";
import { storeToRefs } from "pinia";
import useApi, { useApiRawRequest,apiUrl } from "@/models/api";
import { defineProps, onMounted, watch, ref  } from "vue";
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
const uType = localStorage.getItem("t")
const usrName = localStorage.getItem("username") || "none"
const admin = ref(false)
let failedLogin = ref(false);
let errMsg = ref('')
if(uType == "0"){
  admin.value = true
}

const addName = ref('')
const addEmail = ref('')
let update = ref(false);
let updateId = ref(0);
const updateName = ref('')
const updateEmail = ref('')
function addNew(){
  let notee: Note = {id: 0,
    name: addName.value,
    description: addEmail.value,}
  eventsStore.addNote(notee,usrName)
}
function showUpdate(id:number){
  updateId.value = id
  update.value = true
}
function cancelUpdate(){
  update.value = false
}
function deleteNote(fuck: Note) {
  console.log(fuck)
  eventsStore.deleteNote(fuck,usrName)
  eventsStore.load(usrName);
}
function updatePerson(){
  if(updateId){
    let currentPerson: Note = {
      id:0,
      name:"",
      description:"",
    }
    note.value.forEach(person => {
      if(person.id == updateId.value){
        currentPerson.name = person.name
        currentPerson.description = person.description
      }
    });
    let person: Note = {
      id:updateId.value,
      name:updateName.value || currentPerson.name,
      description:updateEmail.value || currentPerson.description,
    }
    eventsStore.updateNote(person,usrName);
    eventsStore.load(usrName);
    update.value = false

  }
}
const route = useRoute();

watch(route, (to, from) => {
  if (to.path !== from.path || to.query !== from.query) {
    eventsStore.load(usrName);
  }
}, { deep: true });

defineProps<{ title: String }>();
const eventsStore = useNoteStore();
const { note } = storeToRefs(eventsStore);

onMounted(async () => {
  eventsStore.load(usrName);
});
</script>