import { type ReactNode } from 'react';

import Box from '@mui/joy/Box';

import { PageLayout } from '@sisa/components';

import { Sidebar } from 'components/shared/sidebar';
import Header from 'components/shared/header';

type Props = {
  children: ReactNode;
};

const Layout = (props: Props) => {
  return (
    <Box
      sx={{
        display: 'flex',
        minHeight: '100dvh',
        overflowX: 'hidden',
        overflowY: 'auto',
      }}
    >
      <Header />
      <Sidebar />
      <Box
        component="main"
        sx={{
          display: 'flex',
          flex: 1,
          flexDirection: 'column',
          gap: 1,
          minWidth: 0,
          height: '100dvh',
          overflowX: 'hidden',
          overflowY: 'auto',
          pt: {
            xs: `calc(16px + var(--sisa-header-height))`,
            sm: `calc(16px + var(--sisa-header-height))`,
            md: `16px`,
          },
        }}
      >
        <PageLayout>{props.children}</PageLayout>
      </Box>
    </Box>
  );
};

export default Layout;
