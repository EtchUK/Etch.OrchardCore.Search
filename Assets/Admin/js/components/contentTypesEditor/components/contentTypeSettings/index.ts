import Vue from 'vue';

import IContentTypeSettings from '../../models/contentTypeSetting';

export default Vue.extend({
    props: {
        item: {
            type: Object as () => IContentTypeSettings,
        },
    },

    template: '#content-type-settings',
});
