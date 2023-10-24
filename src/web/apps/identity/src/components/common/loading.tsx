import Box from '@mui/joy/Box';

import { LoadingIcon } from '@sisa/components';

const Loading = () => {
  return (
    <Box
      sx={{
        display: 'flex',
        height: '100%',
        width: '100%',
        justifyContent: 'center',
        alignItems: 'center',
      }}
    >
      <LoadingIcon height={40} width={40} />
    </Box>
  );
};

export default Loading;
