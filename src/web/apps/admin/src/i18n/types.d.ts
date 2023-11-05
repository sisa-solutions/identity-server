// import the original type declarations
// import 'i18next';

// import all namespaces (for the default language, only)
// import en from 'locales/en.json';

import { defaultNS } from './configs';
import { resources } from './i18n';

declare module 'i18next' {
  // Extend CustomTypeOptions
  interface CustomTypeOptions {
    defaultNS: typeof defaultNS;
    resources: (typeof resources)['en'];
  }
}
