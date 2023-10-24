import Box, { type BoxProps } from '@mui/joy/Box';

const PageHeader = ({ children, sx, ...rest }: BoxProps) => {
  return (
    <Box
      sx={[
        {
          display: 'flex',
          flexDirection: 'column',
          gap: 1.5,
          px: 2,
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
    >
      {children}
    </Box>
  );
};

export default PageHeader;
