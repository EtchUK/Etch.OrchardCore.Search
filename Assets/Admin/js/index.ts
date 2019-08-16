import contentTypesEditor from './components/contentTypesEditor';

(window as any).initializeContentTypesEditor = (el: HTMLElement): void => {
    const $hiddenDataField = document.getElementById($(el).data('for'));

    if (!$hiddenDataField) {
        return;
    }

    contentTypesEditor($($hiddenDataField).data('init'), el);
};
