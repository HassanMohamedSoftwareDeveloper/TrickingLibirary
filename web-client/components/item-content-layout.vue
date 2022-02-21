<template>
  <div>
    <v-row>
      <v-col cols="12" sm="7" md="6" lg="6" offset-lg="1" xl="4" offset-xl="3">
        <slot name="content"></slot>
      </v-col>
      <v-col class="d-none d-sm-block" sm="5" md="6" lg="4" xl="3">
        <v-sheet class="pa-3 sticky">
          <slot name="item" :close="()=>{}"></slot>
        </v-sheet>
      </v-col>
    </v-row>
    <v-dialog>
      <template v-slot:activator="{ on }" v-model="dialog">
        <v-btn
          v-on="on"
          class="d-sm-none"
          fixed
          fab
          bottom
          right
          small
          color="blue"
        >
          <v-icon>mdi-help-circle-outline</v-icon>
        </v-btn>
      </template>
      <v-sheet class="pa-2" rounded>
        <slot name="item" :close="()=>dialog=false"></slot>
      </v-sheet>
    </v-dialog>
  </div>
</template>

<script>
export default {
  name: "item-content-layout",
  data: () => ({
    dialog: false,
  }),
};
</script>
