import { useField, useFormikContext } from "formik";
import { Form, Label, Dropdown } from "semantic-ui-react";

interface Props {
  placeholder: string;
  name: string;
  label?: string;
  disabled?: boolean;
}

const options = [
  { key: 'lekovita', text: 'Lekovita', value: true },
  { key: 'nelekovita', text: 'Nelekovita', value: false }
];

const ageOptions = [
  { key: 'zaDecu', text: 'Jeste za decu', value: true },
  { key: 'nijeZaDecu', text: 'Nije za decu', value: false }
];

export default function DropdownInput(props: Props) {
  const [field, meta, helpers] = useField(props.name);
  const { setValue } = helpers;
  const { setFieldValue } = useFormikContext();

  const handleChange = (e: any, { value }: any) => {
    setFieldValue(props.name, value);
  };

  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <Dropdown
        placeholder={props.placeholder}
        fluid
        selection
        options={props.name === 'lekovita' || props.name === 'nelekovita' ? options : ageOptions}
        value={field.value}
        onChange={handleChange}
        onBlur={() => helpers.setTouched(true)}
        disabled={props.disabled}
      />
      {meta.touched && meta.error ? (
        <Label basic color="red">{meta.error}</Label>
      ) : null}
    </Form.Field>
  );
}
