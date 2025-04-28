import React, { useState } from 'react';
import { TextField, PrimaryButton } from '@fluentui/react';
import { useDispatch } from 'react-redux';
import { addTodoAsync } from '../features/todoSlice';
import '../App.css';

const AddTodo = () => {
    const [inputValue, setInputValue] = useState('');
    const [deadline, setDeadline] = useState(''); 
    const dispatch = useDispatch();

    const handleAddTodo = () => {
        if (inputValue) {
            dispatch(addTodoAsync({ todoText: inputValue, deadline })); 
            setInputValue('');
            setDeadline(''); 
        }
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
                type="datetime-local" 
                value={deadline}
                onChange={(e) => setDeadline(e.target.value)} 
                placeholder="Set deadline"
                className="add-todo-input"
            />
            <PrimaryButton onClick={handleAddTodo}>Add</PrimaryButton>
        </div>
    );
};

export default AddTodo;
