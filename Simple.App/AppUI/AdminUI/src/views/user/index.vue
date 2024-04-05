<template>
  <div class="app-container"></div>
</template>

<script>
import { mapGetters } from "vuex";
export default {
  name: "usermanager",
  computed: {
    ...mapGetters(["info"]),
  },
  data() {
    return {};
  },

  methods: {},
};
</script>
