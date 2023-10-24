'use client';

import CssBaseline from '@mui/joy/CssBaseline';
import GlobalStyles from '@mui/joy/GlobalStyles';
import { CssVarsProvider } from '@mui/joy/styles';

import CacheProvider from './cache-provider';
import theme from './theme';

type Props = {
  children: React.ReactNode;
};

const ThemeProvider = (props: Props) => {
  return (
    <CacheProvider>
      <CssVarsProvider
        theme={theme}
        // set as root provider
        disableTransitionOnChange
        defaultMode="light"
      >
        <GlobalStyles
          styles={{
            ':root': {
              '--sisa-sidebar-group-zIndex': 1998,
              '--sisa-sidebar-content-zIndex': 1996,
              '--sisa-header-zIndex': 1994,
              '--sisa-sidebar-group-width': '64px',
              '--sisa-sidebar-content-width': '280px',
              '--sisa-header-height': '56px',
            },
            '.lucide': {
              color: 'var(--Icon-color)',
              margin: 'var(--Icon-margin)',
              fontSize: 'var(--Icon-fontSize, 20px)',

              width: '1em',
              height: '1em',
            },

            '.animate-spin': {
              animation: 'spin 2s linear infinite',
            },

            '.animate-blink': {
              animation: 'blink 1s linear infinite',
            },

            '@keyframes spin': {
              '0%': {
                transform: 'rotate(0deg)',
                transformOrigin: '50% 50%',
              },
              '100%': {
                transform: 'rotate(360deg)',
              },
            },

            '@keyframes blink': {
              '0%': {
                opacity: 0.4,
              },
              '50%': {
                opacity: 0.8,
              },
              '100%': {
                opacity: 0.4,
              },
            },
          }}
        />
        <CssBaseline />
        {props.children}
      </CssVarsProvider>
    </CacheProvider>
  );
};

export default ThemeProvider;
