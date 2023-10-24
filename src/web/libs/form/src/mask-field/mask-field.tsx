import { Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import MaskInput, { type MaskInputProps } from '../mask-input';

type MaskFieldProps<
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & MaskInputProps;

const MaskField = <TFieldValues extends FieldValues>({
  name,
  control,
  ...rest
}: MaskFieldProps<TFieldValues>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState: { invalid, error } }) => {
        return (
          <MaskInput
            {...rest}
            {...field}
            error={!!error || invalid}
            errorMessage={error?.message}
          />
        );
      }}
    />
  );
};

export default MaskField;
