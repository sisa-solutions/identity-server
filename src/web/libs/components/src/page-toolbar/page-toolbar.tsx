import Box, { type BoxProps } from '@mui/joy/Box';

const PageToolbar = ({ children, sx, ...rest }: BoxProps) => {
  return (
    <Box
      sx={[
        {
          display: 'flex',
          gap: 1.5,
          flexDirection: { xs: 'column', md: 'row' },
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
    >
      {children}
    </Box>
  );
};

export default PageToolbar;
