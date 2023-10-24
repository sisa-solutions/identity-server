import type { ReactNode } from 'react';

import Box from '@mui/joy/Box';
import { Card, CardContent, Stack, Typography } from '@mui/joy';

import bgLogin from '../../../public/images/bg-login.svg';
import logo from '../../../public/images/logo.png';

interface Props {
  children: ReactNode;
}

const Layout = ({ children }: Props) => {
  return (
    <Box
      display="grid"
      gridTemplateColumns="repeat(12, 1fr)"
      gap={2}
      height="100vh"
      width="100%"
      sx={{
        backgroundImage: `url('${bgLogin.src}')`,
        backgroundRepeat: 'no-repeat',
        backgroundSize: '50%',
        backgroundPosition: '4% center',
        backgroundColor: 'background.level1',
      }}
    >
      <Box
        gridColumn={{
          xs: 'span 0',
          md: 'span 6',
        }}
        display={{
          xs: 'none',
          md: 'block',
        }}
      ></Box>
      <Box
        gridColumn={{
          xs: 'span 12',
          md: 'span 6',
        }}
        sx={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
        }}
      >
        <Stack direction="column" gap={4}>
          <Stack direction="row" gap={2} justifyContent="center" alignItems="center">
            <Box
              width="4rem"
              height="4rem"
              className="animate-blink, animate-spin"
              sx={{
                backgroundImage: `url('${logo.src}')`,
                backgroundRepeat: 'no-repeat',
                backgroundSize: 'contain',
                backgroundPosition: '6%',
              }}
            />
            <Typography level="h1" color="primary">Sisa Identity</Typography>
          </Stack>
          <Card
            variant="outlined"
            color="primary"
            sx={{
              '--Card-padding': {
                xs: '1.5rem',
                sm: '2rem',
              },
              minWidth: {
                xs: '360px',
                sm: '480px',
              },
            }}
          >
            <CardContent>{children}</CardContent>
          </Card>
        </Stack>
      </Box>
    </Box>
  );
};

export default Layout;
