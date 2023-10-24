import Box, { type BoxProps } from '@mui/joy/Box';

const PageFooter = ({ children, sx, ...rest }: BoxProps) => {
  return (
    <Box
      sx={[
        {
          display: 'flex',
          position: 'sticky',
          bottom: 0,
          py: 1,
          px: 2,
          backgroundColor: 'background.level1',
          borderTop: '1px solid',
          borderTopColor: 'divider',
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
    >
      {children}
    </Box>
  );
};

export default PageFooter;
