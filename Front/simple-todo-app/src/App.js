import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { TextField, PrimaryButton, List, IColumn } from '@fluentui/react';
import { addTodo, toggleTodo, deleteTodo } from './features/todoSlice';

const App = () => {
  const [inputValue, setInputValue] = useState('');
  const todos = useSelector((state) => state.todos);
  const dispatch = useDispatch();

  const handleAddTodo = () => {
    if (inputValue) {
      dispatch(addTodo(inputValue));
      setInputValue('');
    }
  };

  const handleToggleTodo = (index) => {
    dispatch(toggleTodo(index));
  };

  const handleDeleteTodo = (index) => {
    dispatch(deleteTodo(index));
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1>Simple To-Do App</h1>
      <TextField
        value={inputValue}
        onChange={(e) => setInputValue(e.target.value)}
        placeholder="Add a new task"
      />
      <PrimaryButton onClick={handleAddTodo}>Add</PrimaryButton>
      <List
        items={todos}
        onRenderCell={(item, index) => (
          <div style={{ display: 'flex', justifyContent: 'space-between', margin: '5px 0' }}>
            <span
              style={{ textDecoration: item.completed ? 'line-through' : 'none', cursor: 'pointer' }}
              onClick={() => handleToggleTodo(index)}
            >
              {item.text}
            </span>
            <PrimaryButton onClick={() => handleDeleteTodo(index)}>Delete</PrimaryButton>
          </div>
        )}
      />
    </div>
  );
};

export default App;

