import Button, { type ButtonProps } from '@mui/joy/Button';

import { CheckCheckIcon, LoaderIcon } from 'lucide-react';

type Props = Omit<ButtonProps, 'onClick'> & {
  submit: Function;
};

const SubmitButton = ({ children, submit, ...rest }: Props) => {
  return (
    <Button
      variant="solid"
      color="primary"
      startDecorator={<CheckCheckIcon />}
      loadingIndicator={<LoaderIcon className="spin" />}
      onClick={() => submit()}
      {...rest}
    >
      {children}
    </Button>
  );
};

export default SubmitButton;
