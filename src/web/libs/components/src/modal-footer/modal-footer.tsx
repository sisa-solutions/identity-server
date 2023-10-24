import Box, { type BoxProps } from '@mui/joy/Box';

const ModalFooter = ({ children, sx, ...rest }: BoxProps) => {
  return (
    <Box
      sx={[
        {
          display: 'flex',
          flexDirection: 'row',
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
    >
      {children}
    </Box>
  );
};

export default ModalFooter;
