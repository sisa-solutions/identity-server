import Box, { type BoxProps } from '@mui/joy/Box';

const FormActions = ({ children, sx, ...rest }: BoxProps) => {
  return (
    <div>
      <div />
      <Box
        sx={[
          {
            display: 'flex',
            flexDirection: 'row',
            gap: 2,
          },
          ...(Array.isArray(sx) ? sx : [sx]),
        ]}
        {...rest}
      >
        {children}
      </Box>
    </div>
  );
};

export default FormActions;
