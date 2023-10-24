import Box, { type BoxProps } from '@mui/joy/Box';

const PageActions = ({ children, sx, ...rest }: BoxProps) => {
  return (
    <Box
      sx={[
        {
          display: 'flex',
          gap: 2,
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

export default PageActions;
