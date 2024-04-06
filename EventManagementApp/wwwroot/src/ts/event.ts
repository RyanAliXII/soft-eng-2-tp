import { createApp, ref } from "vue";
createApp({
  setup() {
    const data = ref<{ name: string }>({ name: "Ryan" });
    return {
      data,
    };
  },
}).mount("#eventPage");
