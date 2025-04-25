// src/components/AddTodo.js
import React, { useState } from 'react';
import { TextField, PrimaryButton } from '@fluentui/react';
import { useDispatch } from 'react-redux';
import { addTodo } from '../features/todoSlice';

const AddTodo = () => {
  const [inputValue, setInputValue] = useState('');
  const dispatch = useDispatch();

  const handleAddTodo = () => {
    if (inputValue) {
      dispatch(addTodo(inputValue));
      setInputValue('');
    }
  };

  return (
    <div>
      <TextField
        value={inputValue}
        onChange={(e) => setInputValue(e.target.value)}
        placeholder="Add a new task"
      />
      <PrimaryButton onClick={handleAddTodo}>Add</PrimaryButton>
    </div>
  );
};

export default AddTodo;
