import React, { useState } from 'react';
import { TextField, PrimaryButton, MessageBar, MessageBarType } from '@fluentui/react'; 
import { useDispatch, useSelector } from 'react-redux';
import { addTodoAsync } from '../features/todoSlice';
import '../App.css';

const AddTodo = () => {
    const [inputValue, setInputValue] = useState('');
    const [deadline, setDeadline] = useState('');
    const [error, setError] = useState(''); 
    const dispatch = useDispatch();
    const todos = useSelector((state) => state.todos); 

    const handleAddTodo = () => {        
        if (!inputValue) {
            setError('Please enter a task.'); 
            return;
        }

        // Check for duplicate todo
        const isDuplicate = todos.some(todo => todo.title.toLowerCase() === inputValue.toLowerCase());
        if (isDuplicate) {
            setError('This todo already exists.');
            return; // Exit the function if duplicate is found
        }
        
        if (!deadline) {
            setError('Please select a deadline.'); 
            return;
        }

        // If no errors, dispatch the action
        dispatch(addTodoAsync({ todoText: inputValue, deadline }));
        setInputValue('');
        setDeadline('');
        setError(''); 
    };

    return (
        <div className="add-todo-container">
            <TextField
                value={inputValue}
                onChange={(e) => setInputValue(e.target.value)}
                placeholder="Add a new task"
                className="add-todo-input"
            />
            <TextField
                type="date" 
                value={deadline}
                onChange={(e) => setDeadline(e.target.value)}
                placeholder="Select a deadline"
                className="add-todo-input"
            />
            <PrimaryButton onClick={handleAddTodo}>Add</PrimaryButton>
            {error && <MessageBar messageBarType={MessageBarType.error}>{error}</MessageBar>} {/* Display error message */}
        </div>
    );
};

export default AddTodo;
