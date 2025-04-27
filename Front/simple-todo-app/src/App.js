import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { TextField, PrimaryButton, DetailsList, DetailsListLayoutMode } from '@fluentui/react';
import { deleteTodoAsync, fetchTodos } from './features/todoSlice'; 
import AddTodo from './components/AddTodo';
import './App.css';

const App = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [filteredTodos, setFilteredTodos] = useState([]);
    const todos = useSelector((state) => state.todos);
    const dispatch = useDispatch();
    
    useEffect(() => {
        dispatch(fetchTodos());
    }, [dispatch]);

    useEffect(() => {
        if (searchTerm) {
            const results = todos.filter(todo =>
                todo.title.toLowerCase().includes(searchTerm.toLowerCase()) 
            );
            setFilteredTodos(results);
        } else {
            setFilteredTodos(todos);
        }
    }, [todos, searchTerm]);
    
    const handleDeleteTodo = (id) => {
        dispatch(deleteTodoAsync(id));
    };

    const columns = [
        {
            key: 'column1',
            name: 'Task',
            fieldName: 'title', 
            minWidth: 200,
            maxWidth: 300,
            isMultiline: true,
            onRender: (item) => (
                <span>
                    {item.title} 
                </span>
            ),
        },
        {
            key: 'column2',
            name: 'Actions',
            fieldName: 'actions',
            minWidth: 100,
            maxWidth: 100,
            onRender: (item) => (
                <PrimaryButton onClick={() => handleDeleteTodo(item.id)}>Delete</PrimaryButton> // Updated: Use 'item.id'
            ),
        },
    ];

    return (
        <div style={{ padding: '20px' }}>
            <h1>Simple To-Do App</h1>
            <AddTodo />
            <div className="search-container">
                <TextField
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    placeholder="Search tasks"
                    className="search-input"
                />
                <PrimaryButton onClick={() => setSearchTerm(searchTerm)}>Search</PrimaryButton>
            </div>
            <DetailsList
                items={filteredTodos}
                columns={columns}
                setKey="set"
                layoutMode={DetailsListLayoutMode.fixedColumns}
                isHeaderVisible={true}
            />
        </div>
    );
};

export default App;
