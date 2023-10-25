'use client';

import Button from '@mui/joy/Button';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

import { ShieldQuestionIcon } from 'lucide-react';

import { FormActions, FormContainer, TextField, useForm, yup, yupResolver } from '@sisa/form';

const LoginWithRecoveryCode = () => {
  const validationSchema = yup.object({
    recoveryCode: yup.string().required().length(8).label('Recovery Code'),
  });

  type FormValues = yup.InferType<typeof validationSchema>;

  const { control, handleSubmit } = useForm<FormValues>({
    defaultValues: {
      recoveryCode: '',
    },
    resolver: yupResolver(validationSchema),
    reValidateMode: 'onBlur',
  });

  const onSubmit = handleSubmit((data: FormValues) => {
    console.log(data);
  });

  return (
    <Stack direction="column" gap={2}>
      <Stack
        direction="column"
        gap={1}
        sx={{
          mb: 2,
        }}
      >
        <Typography level="h3" color="primary">
          Login with Recovery Code
        </Typography>
        <Typography level="body-sm">
          {`If you've lost your device or otherwise cannot use your authenticator app, you can use your recovery code to login.`}
        </Typography>
      </Stack>

      <FormContainer orientation="vertical">
        <TextField
          control={control}
          name="recoveryCode"
          label="Recovery Code"
          required
          placeholder="Enter your recovery code"
        />
        <FormActions display="flex" flex={1} mt={2}>
          <Button
            type="submit"
            variant="solid"
            color="primary"
            sx={{ flex: 1 }}
            onClick={onSubmit}
            startDecorator={<ShieldQuestionIcon />}
          >
            Verify
          </Button>
        </FormActions>
      </FormContainer>
    </Stack>
  );
};

export default LoginWithRecoveryCode;
