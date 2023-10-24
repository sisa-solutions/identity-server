import Box, { type BoxProps } from '@mui/joy/Box';

const PageContent = ({ children, sx, ...rest }: BoxProps) => {
  return (
    <Box
      sx={[
        {
          display: 'flex',
          flex: 1,
          flexDirection: 'column',
          gap: 2,
          overflowX: 'hidden',
          overflowY: 'auto',
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

export default PageContent;
