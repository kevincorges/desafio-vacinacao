import React, { useState } from 'react';
import { useAgendamento } from './AgendamentoContext';
import { api } from './api';

export const ListaAgendamentos: React.FC = () => {
  const { agendamentos, carregando, atualizarAgendamento } = useAgendamento();
  const [editandoId, setEditandoId] = useState<number | null>(null);
  const [statusTemp, setStatusTemp] = useState('');
  const [conclusaoTemp, setConclusaoTemp] = useState('');

  const handleEditarStatus = (id: number, statusAtual: string, conclusaoAtual: string) => {
    setEditandoId(id);
    setStatusTemp(statusAtual);
    setConclusaoTemp(conclusaoAtual || '');
  };

  const handleSalvarStatus = async (id: number) => {
    try {
      await api.atualizarStatus(id, statusTemp, conclusaoTemp);
      atualizarAgendamento(id, { status: statusTemp, conclusao: conclusaoTemp });
      setEditandoId(null);
    } catch (err) {
      console.error('Erro ao atualizar status', err);
    }
  };

  // Agrupar agendamentos por data
  const agrupadosPorData = agendamentos.reduce((acc, agendamento) => {
    const data = agendamento.dataAgendamento;
    if (!acc[data]) {
      acc[data] = [];
    }
    acc[data].push(agendamento);
    return acc;
  }, {} as Record<string, typeof agendamentos>);

  if (carregando) {
    return <div className="text-center py-8">Carregando agendamentos...</div>;
  }

  if (agendamentos.length === 0) {
    return <div className="text-center py-8 text-gray-500">Nenhum agendamento realizado</div>;
  }

  return (
    <div className="bg-white p-6 rounded-lg shadow-md">
      <h2 className="text-2xl font-bold mb-4 text-gray-800">Agendamentos</h2>
      
      {Object.entries(agrupadosPorData).map(([data, items]) => (
        <div key={data} className="mb-6">
          <h3 className="text-lg font-semibold text-blue-600 mb-3">
            {new Date(data).toLocaleDateString('pt-BR')}
          </h3>
          
          <div className="overflow-x-auto">
            <table className="w-full border-collapse">
              <thead>
                <tr className="bg-gray-100">
                  <th className="border p-2 text-left">Paciente</th>
                  <th className="border p-2 text-left">Horário</th>
                  <th className="border p-2 text-left">Status</th>
                  <th className="border p-2 text-left">Conclusão</th>
                  <th className="border p-2 text-left">Ações</th>
                </tr>
              </thead>
              <tbody>
                {items.map(agendamento => (
                  <tr key={agendamento.id} className="hover:bg-gray-50">
                    <td className="border p-2">{agendamento.nomePaciente}</td>
                    <td className="border p-2">{agendamento.horario}</td>
                    <td className="border p-2">
                      {editandoId === agendamento.id ? (
                        <select
                          value={statusTemp}
                          onChange={(e) => setStatusTemp(e.target.value)}
                          className="px-2 py-1 border rounded"
                        >
                          <option value="Pendente">Pendente</option>
                          <option value="Realizado">Realizado</option>
                          <option value="Cancelado">Cancelado</option>
                        </select>
                      ) : (
                        <span className={`px-2 py-1 rounded text-white text-sm ${
                          agendamento.status === 'Pendente' ? 'bg-yellow-500' :
                          agendamento.status === 'Realizado' ? 'bg-green-500' :
                          'bg-red-500'
                        }`}>
                          {agendamento.status}
                        </span>
                      )}
                    </td>
                    <td className="border p-2">
                      {editandoId === agendamento.id ? (
                        <input
                          type="text"
                          value={conclusaoTemp}
                          onChange={(e) => setConclusaoTemp(e.target.value)}
                          className="w-full px-2 py-1 border rounded"
                          placeholder="Conclusão"
                        />
                      ) : (
                        agendamento.conclusao || '-'
                      )}
                    </td>
                    <td className="border p-2">
                      {editandoId === agendamento.id ? (
                        <>
                          <button
                            onClick={() => handleSalvarStatus(agendamento.id)}
                            className="bg-green-500 text-white px-2 py-1 rounded mr-2 text-sm hover:bg-green-600"
                          >
                            Salvar
                          </button>
                          <button
                            onClick={() => setEditandoId(null)}
                            className="bg-gray-500 text-white px-2 py-1 rounded text-sm hover:bg-gray-600"
                          >
                            Cancelar
                          </button>
                        </>
                      ) : (
                        <button
                          onClick={() => handleEditarStatus(agendamento.id, agendamento.status, agendamento.conclusao)}
                          className="bg-blue-500 text-white px-2 py-1 rounded text-sm hover:bg-blue-600"
                        >
                          Editar
                        </button>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      ))}
    </div>
  );
};
