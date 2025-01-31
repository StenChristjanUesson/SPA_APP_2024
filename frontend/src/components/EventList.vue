<template>
  <div class="about">
    <h1>Eventlist</h1>
    <div class="eventlist">
      <DataTable :value="events" v-if="events.length > 0 ">
        <Column field="id" header="Sündmuse id" style="color: black; "/>
        <Column field="type" header="Sündmuse tüüp" style="color: black; "/>
        <Column field="description" header="Kirjeldus" style="color: black; "/>
        <Column field="location" header="Asukoht" style="color: black;"/>
        <Column field="date" header="Kuupäev" style="color: black;"/>
      </DataTable>
      <div v-else>Sündmused puuduvad</div>
    </div>
  </div>
</template>


<script setup lang="ts">
import { type Event } from '@/models/event';
import { useEventsStore } from "@/stores/eventsStore";
import { storeToRefs } from "pinia";
import { defineProps, onMounted, watch, ref  } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();

watch(route, (to, from) => {
  if (to.path !== from.path || to.query !== from.query) {
    eventsStore.load();
  }
}, { deep: true });

defineProps<{ title: String }>();
const eventsStore = useEventsStore();
const { events } = storeToRefs(eventsStore);

onMounted(async () => {
  eventsStore.load();
});
</script>
<style>

</style>