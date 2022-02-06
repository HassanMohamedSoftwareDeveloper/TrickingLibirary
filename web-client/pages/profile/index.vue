<template>
  <item-content-layout>
    <template v-slot:content>
      <div v-if="submissions">
        <v-card class="mb-3" v-for="s in submissions" :key="`${s.id}`">
          <video-player :video="s.video" :key="`v-${s.id}`" />
          <v-card-text>{{ s.description }}</v-card-text>
        </v-card>
      </div>
    </template>
    <template v-slot:item>
      <div v-if="profile">
        <div>
          <input
            class="d-none"
            type="file"
            accept="image/*"
            ref="profileImageInput"
            @change="changeProfileImage"
          />
          <v-hover v-slot:default="{ hover }">
            <v-avatar>
              <v-btn
                icon
                v-if="hover"
                :disabled="uploadingImage"
                @click="$refs.profileImageInput.click()"
              >
                <v-icon>mdi-account-edit</v-icon>
              </v-btn>
              <img v-else-if="profile.image"  :src="profile.image" alt="Profile Image"/>
              <v-icon v-else>mdi-account</v-icon>
            </v-avatar>
          </v-hover>
          {{ profile.username }}
        </div>
      </div>
    </template>
  </item-content-layout>
</template>
<script>
import ItemContentLayout from "@/components/item-content-layout.vue";
import videoPlayer from "@/components/video-player.vue";
import { mapState ,mapMutations} from "vuex";

export default {
  components: { videoPlayer, ItemContentLayout },
  data: () => ({
    submissions: [],
    uploadingImage: false,
  }),
  async mounted() {
    return this.$store.dispatch("auth/_watchUserLoaded", async () => {
      const profile = this.$store.state.auth.profile;
      this.submissions = await this.$axios.$get(
        `/api/user/${profile.id}/submissions`
      );
    });
  },
  methods: {
    changeProfileImage(e) {
      if (this.uploadingImage) return;
      this.uploadingImage = true;
      const fileInput = e.target;
      const formData = new FormData();
      formData.append("image", fileInput.files[0]);
      return this.$axios
        .$put("/api/user/me/image", formData)
        .then((profile) => {
          this.saveProfile({profile});
          fileInput.value="";
          this.uploadingImage = false;
        });
    },
  ...mapMutations("auth",["saveProfile"])
  },
  computed: mapState("auth", ["profile"]),
};
</script>
