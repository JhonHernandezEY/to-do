import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { TextField, PrimaryButton, DetailsList, DetailsListLayoutMode } from '@fluentui/react';
import { deleteTodoAsync, fetchTodos } from './features/todoSlice';
import AddTodo from './components/AddTodo';
import './App.css';

const App = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [filteredTodos, setFilteredTodos] = useState([]);
    const todos = useSelector((state) => state.todos);
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(fetchTodos());
    }, [dispatch]);

    useEffect(() => {
        const results = todos.filter(todo => {
            const matchesName = todo.title.toLowerCase().includes(searchTerm.toLowerCase());
            const matchesStartDate = startDate ? new Date(todo.deadline) >= new Date(startDate) : true;
            const matchesEndDate = endDate ? new Date(todo.deadline) <= new Date(endDate) : true;
            return matchesName && matchesStartDate && matchesEndDate;
        });
        setFilteredTodos(results);
    }, [todos, searchTerm, startDate, endDate]);

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
            name: 'Deadline',
            fieldName: 'deadline',
            minWidth: 150,
            maxWidth: 200,
            onRender: (item) => (
                <span>
                    {item.deadline}
                </span>
            ),
        },
        {
            key: 'column3',
            name: 'Actions',
            fieldName: 'actions',
            minWidth: 100,
            maxWidth: 100,
            onRender: (item) => (
                <PrimaryButton onClick={() => handleDeleteTodo(item.id)}>Delete</PrimaryButton>
            ),
        },
    ];

    return (
        <div style={{ padding: '20px' }}>
            {/* Card for the Title */}
            <div className="card" style={{ padding: '20px', marginBottom: '20px', border: '1px solid #ccc', borderRadius: '8px', textAlign: 'center' }}>
                <h1 style={{ color: 'blue' }}>Simple To-Do App</h1>
            </div>

            {/* Card for AddTodo */}
            <div className="card" style={{ padding: '20px', marginBottom: '20px', border: '1px solid #ccc', borderRadius: '8px' }}>
                <AddTodo />
            </div>

            {/* Card for Search Container */}
            <div className="card" style={{ padding: '20px', border: '1px solid #ccc', borderRadius: '8px' }}>
                <div className="search-container">
                    <TextField
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        placeholder="Search tasks by name"
                        className="search-input"
                    />
                    <TextField
                        type="date"
                        value={startDate}
                        onChange={(e) => setStartDate(e.target.value)}
                        placeholder="Start Date"
                        className="search-input"
                    />
                    <TextField
                        type="date"
                        value={endDate}
                        onChange={(e) => setEndDate(e.target.value)}
                        placeholder="End Date"
                        className="search-input"
                    />
                    <PrimaryButton onClick={() => {}}>Search</PrimaryButton>
                </div>
            </div>

            {/* Details List for Filtered Todos */}
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
