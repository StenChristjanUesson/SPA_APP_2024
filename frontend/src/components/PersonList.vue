<template>
    <div class="about">
      <h1>PersonList</h1>
      <div class="PersonList">
        <DataTable :value="events" v-if="events.length > 0 ">
          <Column field="Id" header="Inimese id" style="color: black; "/>
          <Column field="Name" header="Inimese Nimi" style="color: black; "/>
          <Column field="PhoneNumber" header="Inimese PhoneNumber" style="color: black; "/>
        </DataTable>
        <div v-else>Sündmused puuduvad</div>
      </div>
    </div>
  </template>
  
  
  <script setup lang="ts">
  import { type Person } from '@/models/Person';
  import { usePersonsStore } from "@/stores/PersonStore";
  import { storeToRefs } from "pinia";
  import { defineProps, onMounted, watch, ref  } from "vue";
  import { useRoute } from "vue-router";
  
  const route = useRoute();
  
  watch(route, (to, from) => {
    if (to.path !== from.path || to.query !== from.query) {
        PersonStore.load();
    }
  }, { deep: true });
  
  defineProps<{ title: String }>();
  const PersonStore = usePersonStore();
  const { Person } = storeToRefs(PersonStore);
  
  onMounted(async () => {
    PersonStore.load();
  });
  </script>
  <style>
  
  </style>