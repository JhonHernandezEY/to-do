// src/components/App.js
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { TextField, PrimaryButton, DetailsList, DetailsListLayoutMode } from '@fluentui/react';
import { deleteTodo } from './features/todoSlice';
import AddTodo from './components/AddTodo';

const App = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredTodos, setFilteredTodos] = useState([]);
  const todos = useSelector((state) => state.todos);
  const dispatch = useDispatch(); // Get the dispatch function

  // Effect to refresh the filteredTodos whenever todos or searchTerm changes
  useEffect(() => {
    if (searchTerm) {
      const results = todos.filter(todo =>
        todo.text.toLowerCase().includes(searchTerm.toLowerCase())
      );
      setFilteredTodos(results);
    } else {
      setFilteredTodos(todos);
    }
  }, [todos, searchTerm]);

  const handleDeleteTodo = (index) => {
    dispatch(deleteTodo(index)); // Now dispatch is defined
  };

  const columns = [
    {
      key: 'column1',
      name: 'Task',
      fieldName: 'text',
      minWidth: 200,
      maxWidth: 300,
      isMultiline: true,
      onRender: (item) => (
        <span>
          {item.text}
        </span>
      ),
    },
    {
      key: 'column2',
      name: 'Actions',
      fieldName: 'actions',
      minWidth: 100,
      maxWidth: 100,
      onRender: (item, index) => (
        <PrimaryButton onClick={() => handleDeleteTodo(index)}>Delete</PrimaryButton>
      ),
    },
  ];

  return (
    <div style={{ padding: '20px' }}>
      <h1>Simple To-Do App</h1>
      <AddTodo />
      <TextField
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        placeholder="Search tasks"
        style={{ marginTop: '20px' }}
      />
      <PrimaryButton onClick={() => setSearchTerm(searchTerm)} style={{ marginLeft: '10px' }}>Search</PrimaryButton>
      <DetailsList
        items={filteredTodos} // Always show filtered todos
        columns={columns}
        setKey="set"
        layoutMode={DetailsListLayoutMode.fixedColumns}
        isHeaderVisible={true}
      />
    </div>
  );
};

export default App;
