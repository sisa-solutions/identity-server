import Box, { type BoxProps } from '@mui/joy/Box';

type Props = BoxProps & {
  orientation?: 'vertical' | 'horizontal';
  maxLabelWidth?: string;
};

const FormContainer = (props: Props) => {
  const { orientation = 'vertical', maxLabelWidth = '200px', children, sx, ...rest } = props;

  return (
    <Box
      sx={[
        {
          ...(orientation === 'horizontal'
            ? {
                display: 'grid',
                '& > div': {
                  display: {
                    sm: 'contents',
                  },
                },
                '& > hr': {
                  gridColumn: '1/-1',
                },
                gridTemplateColumns: {
                  xs: '100%',
                  sm: `minmax(80px, ${maxLabelWidth}) 1fr`,
                },
                rowGap: {
                  xs: 2,
                  sm: 2.5,
                },
                columnGap: {
                  xs: 2,
                  sm: 2.5,
                },
              }
            : {
                display: 'flex',
                flexDirection: 'column',
                gap: 2,

                '& > div': {
                  display: 'inherit',
                },
              }),
          ...sx,
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
    >
      {children}
    </Box>
  );
};

export default FormContainer;
