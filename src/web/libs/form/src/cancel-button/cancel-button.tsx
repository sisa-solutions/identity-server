import Button, { type ButtonProps } from '@mui/joy/Button';
import { BanIcon } from 'lucide-react';

type Props = Omit<ButtonProps, 'onClick'> & {
  cancel: Function;
};

const CancelButton = ({ children, cancel, ...rest }: Props) => {
  return (
    <Button
      variant="soft"
      color="primary"
      startDecorator={<BanIcon />}
      onClick={() => cancel()}
      {...rest}
    >
      {children}
    </Button>
  );
};

export default CancelButton;
