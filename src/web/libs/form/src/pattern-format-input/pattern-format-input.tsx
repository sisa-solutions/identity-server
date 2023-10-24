import { forwardRef } from 'react';

import { PatternFormat, type PatternFormatProps } from 'react-number-format';

import Box from '@mui/joy/Box';
import Typography from '@mui/joy/Typography';

import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import FormHelperText from '@mui/joy/FormHelperText';
import Input, { type InputProps } from '@mui/joy/Input';

export type PatternFormatInputProps = Omit<PatternFormatProps, 'customInput'> &
  InputProps & {
    label?: React.ReactNode;
    helperMessage?: React.ReactNode;
    errorMessage?: React.ReactNode;
  };

const PatternNumericFormatInput = forwardRef<HTMLInputElement, PatternFormatProps>(
  function NumericFormatInput(props, ref) {
    return <PatternFormat getInputRef={ref} {...props} />;
  }
);

const PatternFormatInput = forwardRef<HTMLDivElement, PatternFormatInputProps>(
  (
    {
      label,
      helperMessage,
      errorMessage,
      required,
      error,
      disabled,

      // PatternFormatProps
      displayType,
      isAllowed,
      valueIsNumericString,
      renderText,

      allowEmptyFormatting,
      format,
      mask,
      patternChar,

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
                component: PatternNumericFormatInput,
                ...{
                  displayType,
                  isAllowed,
                  valueIsNumericString,
                  renderText,

                  allowEmptyFormatting,
                  format,
                  mask,
                  patternChar,
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

PatternFormatInput.displayName = 'PatternFormatInput';

export default PatternFormatInput;
