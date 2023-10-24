import { Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import TextareaInput, { type TextareaInputProps } from '../textarea-input';

export type TextAreaFieldProps<
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & TextareaInputProps;

const TextAreaField = <TFieldValues extends FieldValues>({
  name,
  control,
  ...rest
}: TextAreaFieldProps<TFieldValues>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState: { invalid, error } }) => {
        return (
          <TextareaInput
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

export default TextAreaField;
