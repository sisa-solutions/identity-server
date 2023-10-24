'use client';

import { type ReactNode } from 'react';
import { usePathname } from 'next/navigation';

import { SWRConfig } from 'swr';

import { useReportWebVitals } from 'next/web-vitals';

import { getInitColorSchemeScript } from '@mui/joy/styles/CssVarsProvider';

import { I18nProvider } from '@sisa/i18n';

import { StoreProvider, createRootStore } from 'stores';

import { i18n } from 'i18n';

import ThemeProvider from 'themes/theme-provider';

type Props = {
  children: ReactNode;
};

const App = (props: Props) => {
  useReportWebVitals((metric) => {
    console.log(metric);
  });

  const pathname = usePathname();

  const rootStore = createRootStore({
    pathName: pathname,
  });

  return (
    <>
      {getInitColorSchemeScript({
        defaultMode: 'light',
      })}
      <StoreProvider value={rootStore}>
        <I18nProvider i18n={i18n}>
          <ThemeProvider>
            <SWRConfig
              value={{
                errorRetryCount: 3,
              }}
            >
              {props.children}
            </SWRConfig>
          </ThemeProvider>
        </I18nProvider>
      </StoreProvider>
    </>
  );
};

export default App;
