import { forwardRef } from 'react';

import { IMaskInput, type IMaskInputProps } from 'react-imask';

import Box from '@mui/joy/Box';
import Typography from '@mui/joy/Typography';

import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import FormHelperText from '@mui/joy/FormHelperText';
import Input, { type InputProps } from '@mui/joy/Input';

const CustomIMaskInput = forwardRef<HTMLInputElement, IMaskInputProps<HTMLInputElement>>(
  function CustomIMaskInput(props, ref) {
    return <IMaskInput inputRef={ref} {...props} />;
  }
);

export type MaskInputProps = Omit<IMaskInputProps<HTMLInputElement>, 'onAccept'> &
  InputProps & {
    label?: React.ReactNode;
    helperMessage?: React.ReactNode;
    errorMessage?: React.ReactNode;
  };

const MaskInput = forwardRef<HTMLDivElement, MaskInputProps>(
  (
    {
      label,
      helperMessage,
      errorMessage,
      required,
      error,
      disabled,
      onChange,

      // PatternFormatProps
      mask,
      format,
      unmask,

      size,
      sx,

      ...inputProps
    },
    ref
  ) => {
    return (
      <FormControl required={required} error={error} disabled={disabled} sx={sx} size={size}>
        <Box>
          {label && <FormLabel>{label}</FormLabel>}
          {helperMessage && <Typography level="body-sm">{helperMessage}</Typography>}
        </Box>
        <Box>
          <Input
            ref={ref}
            slotProps={{
              input: {
                component: CustomIMaskInput,

                onAccept: (value: string) =>
                  // @ts-ignore
                  onChange({
                    target: {
                      // @ts-ignore
                      name: inputProps.name,
                      value,
                    },
                  }),
                ...{
                  mask,
                  unmask,
                  format,
                },
              },
            }}
            {...inputProps}
          />
          {errorMessage && <FormHelperText>{errorMessage}</FormHelperText>}
        </Box>
      </FormControl>
    );
  }
);

MaskInput.displayName = 'MaskInput';

export default MaskInput;
