<template>
  <div class="about">
    <h1>People</h1>
    <div class="peopleList">
      <DataTable :value="people" v-if="people.length > 0 ">
        <Column field="id" header="id" style="color: black; "/>
        <Column field="name" header="Name" style="color: black; "/>
        <Column field="email" header="Email" style="color: black; "/>
        <Column field="eventId" header="EventId" style="color: black;"/>
        <Column header="Actions" style="color: black;">
          <template #body="{ data }">
            <Button type="submit" variant="text" severity="info" label="Uuenda" @click="showUpdate(data.id)"/>
            <Button type="submit" variant="text" severity="danger" label="Kustuta" @click="deletePerson({id:data.id,name:data.name,email:data.email,eventId:data.eventId})"/>
        </template>
        </Column>
      </DataTable>
      <div v-else>Sündmused puuduvad</div>
    </div>
  </div>
  <div>
  <br><br><br>
      <div v-if="!update">
        <InputText name="name" type="text" placeholder="Name" v-model="addName"/><br><br>
        <InputText name="email" type="text" placeholder="Email" v-model="addEmail"/><br><br>
        <InputText name="eid" type="text" placeholder="Event ID" v-model="addEventId"/><br><br>
        <Button type="submit" severity="secondary" label="Lisa uus inimene" @click="addNew"/>
      </div>
      <div v-if="update">
        <InputText name="name" type="text" placeholder="Name" v-model="updateName"/><br><br>
        <InputText name="email" type="text" placeholder="Email" v-model="updateEmail"/><br><br>
        <InputText name="eid" type="text" placeholder="Event ID" v-model="updateEventId"/><br><br>
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
import { type People } from '@/models/people';
import { usePeopleStore } from "@/stores/peopleStore";
import { storeToRefs } from "pinia";
import { defineProps, onMounted, watch, ref  } from "vue";
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';

const addName = ref('')
const addEmail = ref('')
const addEventId = ref('')
let update = ref(false);
let updateId = ref(0);
const updateName = ref('')
const updateEmail = ref('')
const updateEventId = ref('')
function addNew(){
  let people: People = {id: 0,
    name: addName.value,
    email: addEmail.value,
    eventId:parseInt(addEventId.value)}
  eventsStore.addPerson(people)
}
function showUpdate(id:number){
  updateId.value = id
  update.value = true
}
function cancelUpdate(){
  update.value = false
}
function deletePerson(person: People) {
  let people: People = {id: person.id,
    name: "",
    email: "",
    eventId:0}
  eventsStore.deletePerson(people)
  eventsStore.load();
}
function updatePerson(){
  if(updateId){
    let currentPerson: People = {
      id:0,
      name:"",
      email:"",
      eventId:0
    }
    people.value.forEach(person => {
      if(person.id == updateId.value){
        currentPerson.name = person.name
        currentPerson.email = person.email
        currentPerson.eventId = person.eventId
      }
    });
    let person: People = {
      id:updateId.value,
      name:updateName.value || currentPerson.name,
      email:updateEmail.value || currentPerson.email,
      eventId:parseInt(updateEventId.value) || currentPerson.eventId
    }
    eventsStore.updatePerson(person);
    eventsStore.load();
    update.value = false

  }
}
const route = useRoute();

watch(route, (to, from) => {
  if (to.path !== from.path || to.query !== from.query) {
    eventsStore.load();
  }
}, { deep: true });

defineProps<{ title: String }>();
const eventsStore = usePeopleStore();
const { people } = storeToRefs(eventsStore);

onMounted(async () => {
  eventsStore.load();
});
</script>