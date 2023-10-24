import Box, { type BoxProps } from '@mui/joy/Box';

export type PageLayoutProps = BoxProps;

const PageLayout = ({ children, sx, ...rest }: PageLayoutProps) => {
  return (
    <Box
      sx={[
        {
          height: '100dvh',
          overflow: 'hidden',
          display: 'flex',
          flexDirection: 'column',
          gap: 2,
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
    >
      {children}
    </Box>
  );
};

export default PageLayout;
