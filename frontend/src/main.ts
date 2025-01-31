import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

import PrimeVue from 'primevue/config';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import { setApiUrl } from './models/api';


const getRuntimeConf = async () => {
    const runtimeConf = await fetch('/config/runtime-config.json');
    return await runtimeConf.json();
  };
const app = createApp(App)

getRuntimeConf().then((json) => {
    setApiUrl(json.API_URL);
    app.use(createPinia())
    app.use(router)
    app.use(PrimeVue);

    app.component('DataTable', DataTable);
    app.component('Column', Column);

    app.mount('#app')
});