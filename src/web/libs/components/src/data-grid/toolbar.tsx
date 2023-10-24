import { type ReactNode } from 'react';

import Box from '@mui/joy/Box';

type Props = {
  children?: ReactNode;
};

const Toolbar = (props: Props) => {
  return (
    <Box
      sx={{
        display: 'flex',
      }}
    >
      {props.children}
    </Box>
  );
};

export default Toolbar;
