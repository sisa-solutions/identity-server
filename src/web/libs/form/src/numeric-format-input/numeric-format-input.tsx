import { forwardRef } from 'react';

import { NumericFormat, type NumericFormatProps } from 'react-number-format';

import Box from '@mui/joy/Box';
import Typography from '@mui/joy/Typography';

import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import FormHelperText from '@mui/joy/FormHelperText';
import Input, { type InputProps } from '@mui/joy/Input';

export type NumericFormatInputProps = Omit<NumericFormatProps, 'customInput'> &
  InputProps & {
    label?: React.ReactNode;
    helperMessage?: React.ReactNode;
    errorMessage?: React.ReactNode;
  };

const BaseNumericFormatInput = forwardRef<HTMLInputElement, NumericFormatProps>(
  function NumericFormatInput(props, ref) {
    return <NumericFormat getInputRef={ref} {...props} />;
  }
);

const NumericFormatInput = forwardRef<HTMLDivElement, NumericFormatInputProps>(
  (
    {
      label,
      helperMessage,
      errorMessage,
      required,
      error,
      disabled,

      // NumberInputProps
      displayType,
      isAllowed,
      valueIsNumericString,
      renderText,

      thousandSeparator,
      decimalSeparator,
      allowedDecimalSeparators,
      thousandsGroupStyle,
      decimalScale,
      fixedDecimalScale,
      allowNegative,
      allowLeadingZeros,
      suffix,
      prefix,

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
                component: BaseNumericFormatInput,
                ...{
                  displayType,
                  isAllowed,
                  valueIsNumericString,
                  renderText,

                  thousandSeparator,
                  decimalSeparator,
                  allowedDecimalSeparators,
                  thousandsGroupStyle,
                  decimalScale,
                  fixedDecimalScale,
                  allowNegative,
                  allowLeadingZeros,
                  suffix,
                  prefix,
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

NumericFormatInput.displayName = 'NumericFormatInput';

export default NumericFormatInput;
