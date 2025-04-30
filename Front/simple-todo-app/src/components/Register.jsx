import React, { useState } from 'react';
import { TextField, PrimaryButton, Stack, MessageBar, MessageBarType } from '@fluentui/react';

const Register = () => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
  });

  const [errors, setErrors] = useState({});
  const [successMessage, setSuccessMessage] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const validateForm = () => {
    const newErrors = {};
    if (!formData.name) {
      newErrors.name = 'Name is required';
    }
    if (!formData.email) {
      newErrors.email = 'Email is required';
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = 'Email is invalid';
    }
    if (!formData.password) {
      newErrors.password = 'Password is required';
    } else if (formData.password.length < 6) {
      newErrors.password = 'Password must be at least 6 characters';
    }
    return newErrors;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = validateForm();
    if (Object.keys(validationErrors).length === 0) {
      // Submit the form (e.g., send data to an API)
      console.log('Form submitted:', formData);
      setSuccessMessage('Registration successful!');
      // Reset form
      setFormData({ name: '', email: '', password: '' });
      setErrors({});
    } else {
      setErrors(validationErrors);
      setSuccessMessage('');
    }
  };

  return (
    <Stack tokens={{ childrenGap: 15 }} styles={{ root: { maxWidth: 400, margin: 'auto', padding: 20 } }}>
      <h2>Register</h2>
      {successMessage && <MessageBar messageBarType={MessageBarType.success}>{successMessage}</MessageBar>}
      <form onSubmit={handleSubmit}>
        <TextField
          label="Name"
          name="name"
          value={formData.name}
          onChange={handleChange}
          errorMessage={errors.name}
        />
        <TextField
          label="Email"
          name="email"
          type="email"
          value={formData.email}
          onChange={handleChange}
          errorMessage={errors.email}
        />
        <TextField
          label="Password"
          name="password"
          type="password"
          value={formData.password}
          onChange={handleChange}
          errorMessage={errors.password}
        />
        <PrimaryButton type="submit">Register</PrimaryButton>
      </form>
    </Stack>
  );
};

export default Register;
