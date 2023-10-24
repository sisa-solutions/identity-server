import { createI18n } from '@sisa/i18n';
// import { initValidationLocales } from '@sisa/form';

import { resources, defaultNS, fallbackLng, supportedLngs } from './configs';

const i18n = createI18n(resources, fallbackLng, supportedLngs, defaultNS);

// initValidationLocales(i18n);

export default i18n;
