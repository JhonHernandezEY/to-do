import React, { useState } from 'react';
import { TextField, PrimaryButton, Dialog, DialogType, DialogFooter, Icon } from '@fluentui/react';
import { useDispatch, useSelector } from 'react-redux';
import { addTodoAsync } from '../features/todoSlice';
import '../App.css';

const AddTodo = () => {
    const [inputValue, setInputValue] = useState('');
    const [deadline, setDeadline] = useState('');
    const [error, setError] = useState('');
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const dispatch = useDispatch();
    const todos = useSelector((state) => state.todos);

    const handleAddTodo = () => {
        if (!inputValue) {
            setError('Please enter a task.');
            setIsDialogOpen(true);
            return;
        }

        const isDuplicate = todos.some(todo => todo.title.toLowerCase() === inputValue.toLowerCase());
        if (isDuplicate) {
            setError('This todo already exists.');
            setIsDialogOpen(true);
            return;
        }

        if (!deadline) {
            setError('Please select a deadline.');
            setIsDialogOpen(true);
            return;
        }

        dispatch(addTodoAsync({ todoText: inputValue, deadline }));
        setInputValue('');
        setDeadline('');
        setError('');
    };

    const handleCloseDialog = () => {
        setIsDialogOpen(false);
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

            {/* Enhanced Dialog for error messages */}
            <Dialog
                hidden={!isDialogOpen}
                onDismiss={handleCloseDialog}
                dialogContentProps={{
                    type: DialogType.normal,
                    title: (
                        <div style={{ display: 'flex', alignItems: 'center', color: 'red' }}>
                            <Icon iconName="Error" style={{ marginRight: '8px', fontSize: '20px', color: 'red' }} />
                            <span>Error</span>
                        </div>
                    ),
                    subText: (
                        <span style={{ color: 'black' }}>
                            {error}
                        </span>
                    ),
                }}
                modalProps={{
                    isBlocking: true, // Prevent closing by clicking outside
                    styles: { main: { backgroundColor: 'white', padding: '20px', border: 'none' } }, // Set background color to white and remove border
                }}
            >
                <DialogFooter>
                    <PrimaryButton onClick={handleCloseDialog} text="OK" />
                </DialogFooter>
            </Dialog>
        </div>
    );
};

export default AddTodo;
