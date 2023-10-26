import Box, { type BoxProps } from '@mui/joy/Box';
import { forwardRef } from 'react';

type Props = Omit<BoxProps, 'ref'> & {
  orientation?: 'vertical' | 'horizontal';
  maxLabelWidth?: string;
  action?: string;
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';
};

const FormContainer = forwardRef<HTMLFormElement, Props>(
  (
    {
      orientation = 'vertical',
      maxLabelWidth = '200px',
      children,
      sx,
      action,
      method = 'POST',
      ...rest
    }: Props,
    ref
  ) => {
    return (
      <Box
        ref={ref}
        {...(action && {
          action,
          method,
          component: 'form',
        })}
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
  }
);

FormContainer.displayName = 'FormContainer';

export default FormContainer;
