using System.Collections.Generic;
using ToolsWorld.BL.Model;

namespace ToolsWorld.BL.ViewModel
{
    public class WorkerViewModel
    {
        /// <summary>
        /// Коллекция сотрудников.
        /// </summary>
        public List<Worker> Workers { get; set; }

        public WorkerViewModel()
        {
            Workers = Worker.GetWorkers();
        }
    }
}
