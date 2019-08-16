import Vue from 'vue';

import ContentTypeSettings from './components/contentTypeSettings';
import IContentTypeSettings from './models/contentTypeSetting';

export default (initialData: IContentTypeSettings[], element: HTMLElement) => {
    return new Vue({
        el: element,

        components: {
            ContentTypeSettings,
        },

        data: {
            items: [] as IContentTypeSettings[],
        },

        computed: {
            value(): string {
                return JSON.stringify(this.items);
            },
        },

        mounted: function() {
            this.items = initialData;
        },

        methods: {},
    });
};
