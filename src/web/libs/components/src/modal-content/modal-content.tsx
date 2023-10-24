import Box, { type BoxProps } from '@mui/joy/Box';

const ModalContent = ({ children, sx, ...rest }: BoxProps) => {
  return (
    <Box
      sx={[
        {
          display: 'flex',
          flexDirection: 'column',
          flex: 1,
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
    >
      {children}
    </Box>
  );
};

export default ModalContent;
